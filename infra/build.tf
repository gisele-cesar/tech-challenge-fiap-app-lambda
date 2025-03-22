resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command     = <<EOT
      dotnet restore ../lambda/lambdaValidarUsuario.csproj
      dotnet publish ../lambda/lambdaValidarUsuario.csproj -c Release -r linux-x64 --self-contained false -o ../lambda/publish
    EOT
    interpreter = ["PowerShell", "-Command"]
  }
  triggers = {
    always_run = "${timestamp()}"
  }
}