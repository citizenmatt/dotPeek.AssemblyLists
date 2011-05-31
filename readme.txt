To build:

Because dotPeek doesn't yet have an installer, all assemblies are referenced from a dotPeek folder here at the root of the cloned repository.

1. Create a dotPeek directory here, in the root of your cloned repository
2. Copy all of the dotPeek files into this new folder
3. Build the solution. A custom build step will copy the dll into the plugins folder in this new copy of dotPeek
4. Run dotPeek in this folder

Note that the assembly lists are written to a shared workspace file. If you run a version of dotPeek without the plugin, you WILL lose your settings.
