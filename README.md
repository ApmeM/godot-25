# godot-template

## Overview

The HTML5 result can be found here: https://apmem.github.io/godot-template/Godot.html

A template repository to create godot project with the following features:
- Automatic deploy to github pages and google play store on each commit to main branch
- Generate C# code for scenes as a partial class (see https://github.com/ApmeM/GodotFieldsGenerator)
- Incude C# implementations for achievements and popups 
- CommonSignals class contains names for godot signals

## Install

1. Clone repository from template

Replace all occurances of "godotTemplate" to your name

2. Configure android deployment keystores

As an example this repository already has 2 prepared keystore files 

- debug_keystore with password debug_password
- release_keystore with password release_password

Debug keystore can be used on local machine to upload apk on android devices.
Release keystore is used to deploy apk to google play.

To make it work add your relese password to `RELEASE_KEYSTORE_PASSWORD` [repository secret](../../settings/secrets/actions)

4. Web test

Do not forget to enable [Pages](../../settings/pages) and set Source as 'Github Actions'

The page will be available under https://apmem.github.io/godot-template/Godot.html

3. Create keystores for android deployment

Go to "keystore" folder, remove release.keystore and execute following command:

```
keytool -genkey -v -keystore release.keystore -alias release_user -keyalg RSA -keysize 2048 -validity 10000
```

And then add release keystore password to github secrets with the name RELEASE_KEYSTORE_PASSWORD
You can regenerate debug.keystore as well with the following command (see the password is already set):

```
keytool -genkey -v -keystore debug.keystore -alias debug_user -storepass debug_password -keyalg RSA -keysize 2048 -validity 10000
```

4. Create new project in google play 

To add permissions for the github to automatically deploy new versions to play store please check https://github.com/ApmeM/ApmeM/blob/main/github_actions_autodeploy_android_apk.md

Service account json key generated by google console should be copied to github secret with the name SERVICE_ACCOUNT_JSON

5. Enjoy

