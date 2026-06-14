python flatpak-dotnet-generator.py mixi-sources.json ../../Mixi/Mixi.csproj

dotnetpackager flatpak bundle \
 --directory ../../Mixi/bin/Release/net10.0/ \
 --output ../../artifacts/Mixi.flatpak \
 --system \
 --application-name "Mixi" \
 --summary "Mixi - Midi Volume Manager"
