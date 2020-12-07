Ce package permet la mise en place de fichier de traduction pour localiser les différents textes présents dans l'application.

Les fichiers de langues doivent être dans le dossier StreamingAssets/Languages et au format json.

Le script LanguageManager doit être initialisé et présent dans la scène.

Ensuite, il suffit de rajouter le script LanguageEntry sur le composent de TextMeshPro que l'on veut traduire, et renseigner sa clé de traduction.

Le script LanguageManager possède aussi une API pour traduire du texte directement en code pour gérer tout ce qui est dynamique.