resource "null_resource" "build_dotnet_lambda" {
  provisioner "local-exec" {
    command = <<EOT
     '/usr/lib/dotnet/sdk/8.0.114/MSBuild.dll -maxcpucount -verbosity:m -restore -target:Publish --property:_IsPublishing=true -property:PublishDir=../lambdaValidarUsuario/publish -property:_CommandLineDefinedOutputPath=true -property:SelfContained=False -property:_CommandLineDefinedSelfContained=true -property:RuntimeIdentifier=linux-x64 -property:_CommandLineDefinedRuntimeIdentifier=true -property:Configuration=Release -property:DOTNET_CLI_DISABLE_PUBLISH_AND_PACK_RELEASE=true ../lambdaValidarUsuario/lambdaValidarUsuario.csproj'
    EOT
    interpreter = ["/bin/sh", "-c"]
  }
}

