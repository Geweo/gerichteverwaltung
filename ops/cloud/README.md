# Cloud Infrastructure

Dieses Verzeichnis enthält die Cloud-Infrastructure-Definitionen für Ernährbär.

## Status

**Aktuell:** Noch nicht implementiert. Dieses Verzeichnis ist für zukünftige Infrastructure-as-Code (IaC) vorbereitet.

## Geplante Inhalte

- **Pulumi** oder **Terraform** für Infrastructure-as-Code
- Cloud-Deployment-Konfigurationen (AWS, Azure, etc.)
- Environment-spezifische Konfigurationen (Dev, Staging, Prod)

## Referenz

Die Architektur orientiert sich an der Zentreo-Struktur:
- `app-10-infrastructure/` - Basis-Infrastructure (ECR, Roles, etc.)
- `app-20-environment/` - Environment-spezifische Ressourcen
- `app-50-payload/` - Application-Deployments
