#!/bin/sh

python3 flatpak-dotnet-generator.py ../flatpak/mixi-sources.json ../../Mixi/Mixi.csproj --runtime linux-x64 --dotnet-args --no-cache --verbosity detailed
flatpak-builder --force-clean --user --install-deps-from=flathub --repo=repo --install builddir ../flatpak/cpaz.flatpak.Mixi.yml
