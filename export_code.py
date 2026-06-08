from pathlib import Path
from datetime import datetime

OUTPUT_FILE = "bulkbuddy_code_export.txt"

INCLUDE_EXTENSIONS = {
    ".cs",
    ".cshtml",
    ".json",
    ".sql",
    ".csproj",
    ".sln",
    ".slnx",
    ".txt",
}

EXCLUDE_DIRS = {
    ".git",
    "bin",
    "obj",
    "node_modules",
    "wwwroot/lib",
    "BulkBuddyV3_backup",
    ".vs",
    ".vscode",
}

EXCLUDE_FILES = {
    "bulkbuddy_code_export.txt",
    "export_code.py",
}

def should_exclude(path: Path) -> bool:
    parts = set(path.parts)

    for excluded in EXCLUDE_DIRS:
        excluded_parts = set(Path(excluded).parts)
        if excluded_parts.issubset(parts):
            return True

    if path.name in EXCLUDE_FILES:
        return True

    return False


def main() -> None:
    root = Path.cwd()
    output_path = root / OUTPUT_FILE

    files = []

    for path in root.rglob("*"):
        if not path.is_file():
            continue

        relative_path = path.relative_to(root)

        if should_exclude(relative_path):
            continue

        if path.suffix.lower() in INCLUDE_EXTENSIONS:
            files.append(relative_path)

    files.sort(key=lambda p: str(p).lower())

    with output_path.open("w", encoding="utf-8") as output:
        output.write("BulkBuddy Code Export\n")
        output.write(f"Generated at: {datetime.now()}\n")
        output.write(f"Project path: {root}\n")
        output.write("\n")

        output.write("==============================\n")
        output.write("PROJECT STRUCTURE\n")
        output.write("==============================\n\n")

        for file in files:
            output.write(f"{file}\n")

        output.write("\n\n")
        output.write("==============================\n")
        output.write("CODE FILES\n")
        output.write("==============================\n")

        for file in files:
            file_path = root / file

            output.write("\n\n")
            output.write("==================================================\n")
            output.write(f"FILE: {file}\n")
            output.write("==================================================\n\n")

            try:
                content = file_path.read_text(encoding="utf-8")
            except UnicodeDecodeError:
                content = file_path.read_text(encoding="latin-1")
            except Exception as error:
                content = f"[ERROR READING FILE: {error}]"

            output.write(content)

    print(f"Export complete: {output_path}")
    print(f"Files exported: {len(files)}")


if __name__ == "__main__":
    main()