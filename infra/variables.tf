variable "state_bucket_name" {
  description = "Name of the S3 bucket for storing Terraform state"
  type = string
}
variable "state_key_file" {
  description = "state key file"
  type = string
}

variable "aws_region" {
  description = "AWS region"
  type = string
}

variable "env" {
  description = "Environment dev, prod"
  type = string
}