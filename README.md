# SCPakTool

A command-line tool for unpacking, repacking, and inspecting Survivalcraft 2 `.pak` files.

## Features

* **Unpack** `.pak` archives into a structured folder format
* **Repack** from a folder structure using `manifest.json`
* **Inspect** asset metadata, including original engine type names
* Supports unserializing certain binary asset types for easier editing

## Usage

```bash
# Unpack a PAK file
scpaktool --unpack myfile.pak output_folder

# Repack from a folder
scpaktool --pack myfolder myfile.pak

# Inspect metadata
scpaktool --inspect myfile.pak
```

## Folder Structure

When unpacked, the `.pak` will be extracted into a folder containing:

```
/assets/...        # Game assets by category/type
manifest.json      # Metadata and build info
```

## Requirements

* .NET 8.0+

## License

MIT License
