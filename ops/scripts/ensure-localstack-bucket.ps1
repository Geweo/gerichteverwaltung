# Erstellt den S3-Bucket "ernaehrbar-uploads" in LocalStack (lokal).
# Einmalig ausführen nach: docker-compose up -d localstack
# Voraussetzung: AWS CLI (aws s3) installiert, z.B. via: pip install awscli

$endpoint = "http://localhost:4566"
$bucket = "ernaehrbar-uploads"
$region = "eu-central-1"

Write-Host "Erstelle Bucket $bucket in LocalStack ($endpoint)..." -ForegroundColor Cyan
try {
    aws s3 mb "s3://$bucket" --endpoint-url $endpoint --region $region 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Bucket $bucket angelegt." -ForegroundColor Green
    } else {
        # 409 = BucketExists
        Write-Host "Hinweis: Bucket existiert ggf. bereits." -ForegroundColor Yellow
    }
} catch {
    Write-Host "Fehler. Stelle sicher: 1) LocalStack laeuft (docker-compose up -d localstack), 2) AWS CLI installiert (aws s3)." -ForegroundColor Red
}
