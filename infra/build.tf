resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command = <<EOT
        dotnet restore "./lambdaValidarUsuario/lambdaValidarUsuario.csproj"  -t:rebuild
        dotnet publish "./lambdaValidarUsuario/lambdaValidarUsuario.csproj"  -t:rebuild -c Release -r linux-x64 --self-contained false -o ../publish
    EOT
    interpreter = ["/bin/sh", "-c"]
  }
}

