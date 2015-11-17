# Outil de conversion Export XML vers Site HTML

Cet outil permet, à partir d'un export brut du WIKI Pathfinder, de générer un site complet au format HTML.

Il est utilisé, entre autre, pour la génération du site http://regles-pathfinder.fr à partir de l'export du wiki pathfinder.

Il nécessite que le dossier "In" contienne une extraction du wiki pathfinder.

## Comment générer sa propre version du DRP ?

Ce guide liste les étapes nécessaire pour générer sa propre version du DRP en se basant sur un export du wiki Pathfinder-fr.

### 1. Génération de l'outil

Pour cette étape, vous avez besoin d'une installation de Visual Studio ou du Framework 4.0.

Lancez simplement la commande "ClicktoBuild.cmd" qui devrait vous générer directement l'outil.

Si la génération s'est bien passée, le dossier "Bin" devrait contenir le programme "XmlToHtml.exe".

### 2. Récupération du dernier export wiki

Téléchargez le dernier export wiki depuis l'adresse http://db.pathfinder-fr.org/raw/WikiXml.7z

Décompressez ensuite les fichiers dans un dossier "In", et assurez-vous de déplacer les fichiers du dossier "Out" vers le dossier parent.
A la fin, vous devriez obtenir la structure suivante : In\Pathfinder-RPG\Aasimar.html (par exemple).

### 3. Génération du site

Lancez ensuite simplement le script "Generate.bat". Vous devriez avoir à la fin un dossier "Out" qui contiendra les fichiers du site.