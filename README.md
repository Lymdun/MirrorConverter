# Unet to Mirror Converter

This is a tool which automatically convert your UNET project to Mirror project, including components, prefabs and scripts.  
Based on Mirror [migration guide](https://vis2k.github.io/Mirror/General/Migration), this tool is able to do alone the following steps: 
- Replace namespace
- Remove NetworkSettings
- Rename SyncListStruct to SyncList
- Replace NetworkHash128 and NetworkInstanceId
- Replace Components
- Replace Usings

Others steps are needed to be done by you.

_In case you want to exclude a script from the Script Converter, just add **//MirrorConverter NoConversion** on top of your script._

## Installation

Simply download latest [release](https://github.com/Lymdun/MirrorConverter/releases) and import it on Unity.
Or, you can download this repo, and put the inside of Assets\Mirror inside the Mirror folder of your project. 

## Usage

Just go through **Tools\Mirror** on Unity3D with your project open.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)
