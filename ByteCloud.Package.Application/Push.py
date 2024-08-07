import os
import subprocess

# Get the directory containing the script
script_dir = os.path.dirname(os.path.realpath(__file__))

# Set the NuGet source URL for GitHub Packages
source_url = "https://nuget.pkg.github.com/Dajnomite/index.json"

# Specify a custom version for the package
custom_version = "1.0.0"

# Find the name of the .csproj file in the script directory
project_name = None
for file_name in os.listdir(script_dir):
    if file_name.endswith(".csproj"):
        project_name = file_name[:-7]
        break

# Raise an exception if no .csproj file is found
if project_name is None:
    raise Exception("No .csproj file found in the script directory")

# Construct the path to the .csproj file
csproj_path = os.path.join(script_dir, f"{project_name}.csproj")

# Delete all other .nupkg files in the release directory
release_dir = os.path.join(script_dir, "bin/Release")

# Check if the release directory exists, if not, create it
if not os.path.isdir(release_dir):
    os.makedirs(release_dir)

# Delete all other .nupkg files in the release directory
for file_name in os.listdir(release_dir):
    if file_name.endswith(".nupkg"):
        os.remove(os.path.join(release_dir, file_name))

# Restore the NuGet packages
restore_command = f'dotnet restore "{csproj_path}"'
try:
    subprocess.run(restore_command, shell=True, check=True)
    print("Restore command executed successfully.")
except subprocess.CalledProcessError:
    print("Restore command failed. Exiting.")
    exit(1)

# Pack the package with the custom version number and create a .nupkg file
pack_command = f'dotnet pack "{csproj_path}" --configuration Release /p:Version={custom_version}'
try:
    subprocess.run(pack_command, shell=True, check=True)
    print("Pack command executed successfully.")
except subprocess.CalledProcessError:
    print("Pack command failed. Exiting.")
    exit(1)

# Find the name of the newly created package file
package_file_name = None
for file_name in os.listdir(release_dir):
    if file_name.endswith(".nupkg"):
        package_file_name = file_name
        break

# Ensure package file is found before proceeding
if package_file_name is None:
    print("No package file found. Exiting.")
    exit(1)


# Set up the push command with authentication
push_command = f'dotnet nuget push "{os.path.join(release_dir, package_file_name)}" --source {source_url}'
try:
    subprocess.run(push_command, shell=True, check=True)
    print("Push command executed successfully.")
except subprocess.CalledProcessError:
    print("Push command failed. Exiting.")
    exit(1)

# Print a message when the script has finished running
print("Done")

# Exit with status code 1337 to signify a successful run
exit(1337)
