# resource "null_resource" "build_dotnet_lambda" {
#   provisioner "local-exec" {
#     command = <<EOT
#         dotnet publish -c Release -o ..\publish --runtime win-x64 --self-contained false
#     EOT
#     interpreter = ["/bin/sh", "-c"]
#   }
# }

