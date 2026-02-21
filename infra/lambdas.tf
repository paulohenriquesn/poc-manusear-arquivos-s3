resource "aws_iam_role" "lambda-role" {
  name = "${var.env}-lambda-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action = "sts:AssumeRole"
      Effect = "Allow"
      Principal = {
        Service = "lambda.amazonaws.com"
      }
    }]
  })
}


resource "aws_iam_role_policy_attachment" "lambda_logs" {
  role       = aws_iam_role.lambda-role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_iam_role_policy_attachment" "sqs_access" {
  role       = aws_iam_role.lambda-role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSQSFullAccess"
}

resource "aws_iam_role_policy_attachment" "awslambdaexecute" {
  role       = aws_iam_role.lambda-role.name
  policy_arn = "arn:aws:iam::aws:policy/AWSLambdaExecute"
}

resource "aws_cloudwatch_log_group" "lambda_log_group" {
  name              = "/aws/lambda/${var.env}-mapear-dados-arquivos"
  retention_in_days = 7
}

resource "aws_lambda_function" "mapear-dados-arquivos" {
  function_name = "${var.env}-mapear-dados-arquivos"
  filename      = "${path.module}/packages/MapearDadosArquivo.zip"
  runtime       = "dotnet10" 
  handler       = "MapearDadosArquivo::MapearDadosArquivo.Function::FunctionHandler" 
  role          = aws_iam_role.lambda-role.arn
  source_code_hash = filebase64sha256("${path.module}/packages/MapearDadosArquivo.zip")
  timeout = 60
  memory_size = 512

  depends_on = [
    aws_cloudwatch_log_group.lambda_log_group,
    aws_iam_role_policy_attachment.lambda_logs
  ]
}

resource "aws_lambda_event_source_mapping" "sqs_to_lambda" {
  event_source_arn = aws_sqs_queue.processar-arquivos-queue.arn
  function_name    = aws_lambda_function.mapear-dados-arquivos.arn
  batch_size       = 1
  enabled          = true

  depends_on = [
    aws_sqs_queue.processar-arquivos-queue,
    aws_lambda_function.mapear-dados-arquivos
  ]
}