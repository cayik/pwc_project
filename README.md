# PWC - Project
## Projektbeschreibung
Dieses Projekt ist eine Beispielanwendung für eine Webanwendung, die verschiedene Funktionen wie Benutzerregistrierung, Benutzeranmeldung, Monster- und Ausrüstungsverwaltung sowie Authentifizierung über JSON Web Tokens (JWT) bietet. Die Anwendung ist in ASP.NET Core erstellt und verwendet eine MySQL-Datenbank für die Speicherung von Benutzerdaten, Monsterinformationen und Ausrüstungsdaten.

## Funktionen
- **Benutzerregistrierung und -anmeldung**: Benutzer können sich in der Anwendung registrieren und anmelden. Die Registrierung erfolgt über einen Benutzernamen, eine E-Mail-Adresse und ein Passwort. Die Anmeldung erfolgt mit den registrierten Anmeldeinformationen.
- **Monsterverwaltung**: Die Anwendung ermöglicht das Abrufen von Informationen zu Monstern und deren Beute. Benutzer können Monster und deren Beute abrufen.
- **Ausrüstungsverwaltung**: Benutzer können Informationen zu Ausrüstungsgegenständen abrufen, darunter Kategorien, Statistiken und Ausrüstungsplätze. Des Weiteren können Benutzer Gegenstände ausrüsten.
- **Authentifizierung über JWT**: Die Anwendung verwendet JWTs zur Authentifizierung von Benutzern. Benutzer erhalten nach der Anmeldung ein JWT-Token, das sie für den Zugriff auf geschützte Endpunkte verwenden können.

## Ausführen

### Secrets.json

**`secrets.json`-Datei erstellen:** Erstellen Sie eine `secrets.json`-Datei im Root-Verzeichnis des Projekts mit folgender Struktur:

```json
{
  "JWT_SECRET": "",
  "SERVER_IP": "",
  "USER_NAME": "root",
  "USER_PASSWORD": "",
  "DATABASE_NAME": ""
}
```
- JWT_SECRET: Ein Geheimnis (Secret), das für die Erstellung von JWTs verwendet wird. (muss > 256 bits sein)
- SERVER_IP: Die IP-Adresse oder der Hostname des MySQL-Datenbankservers. Wenn Sie Docker Compose verwenden, setzen Sie diesen Wert auf "db".
- USER_NAME: Der Benutzername für den Zugriff auf die MySQL-Datenbank --> Default auf Root, wenn es nicht der Root user ist muss ein User mit Rechten für die Datenbank erstellt werden.
- USER_PASSWORD: Das Passwort für den MySQL-Datenbankbenutzer.
- DATABASE_NAME: Der Name der MySQL-Datenbank, in der die Anwendungsdaten gespeichert werden.

### Starten ohne Docker-Compose (Lokale MySQL-Installation erforderlich)
1. Projekt einrichten: Stellen Sie sicher, dass Sie ASP.NET Core auf Ihrem System installiert haben.

2. Datenbank einrichten: Erstellen Sie die erforderlichen Datenbanktabellen, indem Sie die WoW-Best-In-Slot.sql DUMP-Datei in die MySQL-Datenbank laden. (gehen sie sicher, dass alle Eingaben in der secrets.json korrekt sind)

3. Starten der Anwendung: Starten Sie die Anwendung in Visual Studio oder einer beliebigen Entwicklungsumgebung, indem Sie die Anwendung ausführen und auf die entsprechenden Endpunkte navigieren. Die API-Dokumentation ist über Swagger verfügbar und kann über http://ip:port/swagger/index.html erreicht werden.

### Starten mit Docker-Compose
1. Projekt einrichten: Stellen Sie sicher, dass Sie Docker und Docker Compose auf Ihrem System installiert haben.

2. Datenbankcontainer erstellen: Verwenden Sie docker-compose up --build, um die Anwendung zusammen mit einem MySQL-Datenbankcontainer zu starten. Die MySQL-Containerkonfiguration ist bereits in der docker-compose.yml-Datei vorhanden und muss gegebnfalls verändert werden. In der docker-compose.yml müssen "MYSQL_ROOT_PASSWORD" und "MYSQL_DATABASE" gleich wie in der secrets.json heißen.

3. Starten der Anwendung: Sobald die Container gestartet sind, ist die Anwendung verfügbar. Sie können die API-Dokumentation über Swagger über http://ip:port/swagger/index.html erreichen.


### Verwenden von Swagger mit JWT-Token

Diese Anwendung verwendet Swagger zur Dokumentation der API-Endpunkte. Wenn Sie JWT-Token in Swagger verwenden möchten, befolgen Sie diese Schritte:

1. Starten Sie die Anwendung gemäß den oben beschriebenen Anweisungen (entweder mit oder ohne Docker-Compose).

2. Navigieren Sie in Ihrem Webbrowser zur Swagger-Oberfläche, die unter `http://ip:port/swagger/index.html` verfügbar ist, wobei `ip` die IP-Adresse oder der Hostname Ihres Webservers und `port` der Port der Anwendung ist.

3. In der Swagger-Oberfläche finden Sie oben rechts die Schaltfläche "Authorize". Klicken Sie darauf.

4. Ein Dialogfeld zum Hinzufügen von Sicherheitsdefinitionen wird angezeigt. Geben Sie im Textfeld `bearer` ein und fügen Sie dann Ihren JWT-Token ein, den Sie erhalten haben, nachdem Sie sich erfolgreich über die Benutzeranmeldung (`/user/login`) authentifiziert haben.

Beispiel: 
```
bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiZXhwIjoxNzA0MDUyMDA3fQ.Onvg1bx8VRf0FwTR8FZWCg99jz3v-u0QKh62sji8RBU
```

5. Klicken Sie auf die Schaltfläche "Authorize", um die Sicherheitsdefinition hinzuzufügen.

6. Nun können Sie die geschützten Endpunkte der API über Swagger aufrufen und den JWT-Token verwenden, um die Authentifizierung durchzuführen. Swagger fügt automatisch den Token im Header Ihrer Anfragen hinzu.

Bitte beachten Sie, dass dies nur zu Demonstrationszwecken ist. In einer produktiven Umgebung sollten Sie geeignete Sicherheitsmaßnahmen ergreifen und den JWT-Token sicher verwalten.

![](./img/bearer.png)


Bei Fragen oder Problemen können Sie uns gerne eine E-Mail an cayan.ayik1@gmail.com senden.

## Abhängigkeiten
- ASP.NET Core
- MySQL
- Pomelo.EntityFrameworkCore.MySql
- BCrypt.Net
- Swashbuckle.AspNetCore
- Microsoft.AspNetCore.Authentication.JwtBearer

