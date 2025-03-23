resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command = <<EOT
        dotnet restore "./lambdaValidarUsuario/lambdaValidarUsuario.csproj"  -t:rebuild
        dotnet publish "./lambdaValidarUsuario/lambdaValidarUsuario.csproj"  -c Release -r linux-x64 --self-contained false -o "/tech-challenge-fiap-app-lambda/publish"
    EOT
    interpreter = ["/bin/sh", "-c"]
  }
}

