resource "aws_lambda_function" "lambda" {
  filename         = "../package.zip"
  function_name    = "lambda.ValidarUsuario"
  role             = aws_iam_role.lambda.arn
  handler          = "lambda.ValidarUsuario::lambda.ValidarUsuario.LambdaHandler::handleRequest" #Class is build from a source generator
  runtime          = "dotnet8"
  architectures    = ["x86_64"]
  memory_size      = "512"
  timeout          = 10

  environment {
    variables = {
      VAR1 = "VAR1",
      VAR2 = "VAR2"
    }
  }
}

resource "aws_iam_role" "lambda" {
  name                = "${var.component_name}-${data.aws_region.current.name}"
  assume_role_policy  = data.aws_iam_policy_document.assume_role_policy.json
}

data "aws_iam_policy_document" "assume_role_policy" {
  version = "2012-10-17"
  statement {
    actions = [
      "sts:AssumeRole"
    ]
    principals {
      type        = "Service"
      identifiers = [
        "lambda.amazonaws.com"
        ]
    }
  }
}

# Lambda execution

data "aws_iam_policy" "lambdabasic" {
  arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_iam_role_policy_attachment" "lambdabasic" {
  role       = aws_iam_role.lambda.name
  policy_arn = data.aws_iam_policy.lambdabasic.arn
}

resource "aws_apigatewayv2_api" "lambda" {
  name          = "dotnet-lambda-annotations"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_stage" "lambda" {
  api_id = aws_apigatewayv2_api.lambda.id

  name        = "dotnet-lambda-annotations"
  auto_deploy = true
}

resource "aws_apigatewayv2_integration" "dotnet" {
  api_id = aws_apigatewayv2_api.lambda.id

  integration_uri    = aws_lambda_function.lambda.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

resource "aws_apigatewayv2_route" "dotnet" {
  api_id = aws_apigatewayv2_api.lambda.id

  route_key = "POST /orders/{userId}/create"
  target    = "integrations/${aws_apigatewayv2_integration.lambda.id}"
}

resource "aws_lambda_permission" "api_gw" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.lambda.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.lambda.execution_arn}/*/*"
}