# Ghid de Instalare Aplicație

## A) Cerințe preliminare:
- IIS (Internet Information Services) instalat, la care se adaugă .NET Core Hosting Bundle (vezi [aici](https://learn.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-7.0&tabs=visual-studio))
- SQL Server instalat.

  Atenție:
  - Scriptul SQL conține căi absolute (modificați pentru a se potrivi cu sistemul dumneavoastră)
  - Scriptul SQL conține instrucțiuni pentru crearea unei baze de date cu numele "Catalog" (atenție la conflicte și modificați dacă doriți alt nume)
  - Fișierul "ASPDotNetApp.exe" conține instanțe ale tuturor aplicațiilor necesare pentru testare (fără a fi necesari pașii 4 și 5 de mai jos)

## B) Etape:
### Pasul 1: Extrage Fișierele
Extrageți `Catalog.zip` într-o locație la alegerea dvs.

### Pasul 2: Configurați Baza de Date
- Deschideți SQL Server Management Studio.
- Conectați-vă la instanța dvs. SQL Server.
- Încărcați scriptul SQL din fișierul `catalog.sql`.
- Executați fișierul `catalog.sql`.
- Efectuați un Refresh pe Object Explorer pentru a vă asigura că baza de date și tabelele s-au creat.

### Pasul 3: Modificați Șirurile de Conectare
- Deschideți fișierul `appsettings.json` din folderul `extras`.
- Localizați șirul de conectare și schimbați marcatorii:
```
Server=NUMELE_SERVERULUI_DVS,
Database=NUMELE_BAZEIDVS,
User Id=NUMELE_UTILIZATORULUI_DVS,
Password=PAROLA_DVS
```

### Pasul 4: Implementare în IIS
- Deschideți Managerul IIS.
- Faceți clic dreapta pe `Sites` și alegeți `Add Website`.
- Pentru `Physical Path`, selectați folderul unde ați extras `MyWebApp`.
- Atribuiți un `Site name` și selectați un `Port`.
- Faceți clic pe `OK`.

### Pasul 5: Navigați pe Site
- Deschideți un browser web și accesați `http://localhost:PORTUL_DVS` pentru a accesa aplicația web.

## C) Pentru vizualizarea surselor vizitați: [adresa GitHub a proiectului](https://github.com/TudorDan/ASPDotNetApp.git)

Gata! Aplicația dvs. web ar trebui să fie acum funcțională.
