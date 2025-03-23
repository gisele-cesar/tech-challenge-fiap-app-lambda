resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command = <<EOT
        dotnet restore ../../../../../lambdaValidarUsuario.csproj
        dotnet publish ../lambdaValidarUsuario.csproj -c Release -r linux-x64 --self-contained false -o ../publish
    EOT
    interpreter = ["/bin/sh", "-c"]
  }
}

