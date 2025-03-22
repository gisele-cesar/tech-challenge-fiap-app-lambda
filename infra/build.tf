resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command = <<EOT
      dotnet restore lambdaValidarUsuario/lambdaValidarUsuario.csproj
      dotnet publish lambdaValidarUsuario/lambdaValidarUsuario.csproj -c Release -r linux-x64 --self-contained false -o lambdaValidarUsuario/publish
    EOT
    interpreter = ["/bin/sh", "-c"]
  }
}