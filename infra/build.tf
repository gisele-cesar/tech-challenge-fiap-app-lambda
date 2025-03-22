resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command = <<EOT
      dotnet restore ../lambdaValidarUsuario.csproj
    EOT
    interpreter = ["/bin/sh", "-c"]
  }
}

