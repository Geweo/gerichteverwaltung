#!/usr/bin/env bash
# Erstellt den S3-Bucket "ernaehrbar-uploads" in LocalStack (lokal).
# Einmalig ausführen nach: docker-compose up -d localstack
# Voraussetzung: AWS CLI (aws s3) installiert

set -e
ENDPOINT="http://localhost:4566"
BUCKET="ernaehrbar-uploads"
REGION="eu-central-1"

echo "Erstelle Bucket $BUCKET in LocalStack ($ENDPOINT)..."
aws s3 mb "s3://$BUCKET" --endpoint-url "$ENDPOINT" --region "$REGION" || true
echo "Fertig."
