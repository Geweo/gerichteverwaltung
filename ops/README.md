# Operations & Infrastructure

Dieses Verzeichnis enthält alle Operations- und Infrastructure-bezogenen Konfigurationen und Scripts für Ernährbär.

## Struktur

```
ops/
├── local/          # Lokale Entwicklungsumgebung
│   ├── supabase/   # Lokale Supabase-Konfiguration
│   ├── postgres/   # Postgres Init Scripts (für später)
│   └── localstack/ # LocalStack S3 Konfiguration
├── cloud/          # Cloud Infrastructure (IaC - für später)
├── pipeline/       # CI/CD Pipeline Konfigurationen (für später)
└── scripts/        # Utility Scripts
```

## Lokale Entwicklung

### Supabase

Supabase wird lokal über die Supabase CLI gestartet:

```bash
cd ops/local/supabase
pnpm install
pnpm run start
```

Oder verwende das Helper-Script im Projektroot:
```bash
./supabase-restart.sh
```

### LocalStack

LocalStack wird über Docker Compose gestartet (siehe Root `docker-compose.yml`).

Um den S3-Bucket zu erstellen:
```bash
cd ops/scripts
./ensure-localstack-bucket.sh
# oder
./ensure-localstack-bucket.ps1
```

## Cloud Infrastructure

Das `cloud/` Verzeichnis ist für zukünftige Infrastructure-as-Code (IaC) vorbereitet. Siehe [cloud/README.md](cloud/README.md) für Details.

## Pipeline

Das `pipeline/` Verzeichnis ist für zukünftige CI/CD-Pipeline-Konfigurationen vorbereitet. Siehe [pipeline/README.md](pipeline/README.md) für Details.

## Referenz

Die Struktur orientiert sich an der bewährten Architektur von Zentreo (`tp_dym-zentreo-v1`).
