# Unet to Mirror Converter

This is a tool which automatically convert your UNET project to Mirror project, including components, prefabs and scripts.  
Based on Mirror [migration guide](https://vis2k.github.io/Mirror/General/Migration), this tool is able to do alone the following steps: 
- 3. Replace namespace
- 4. Remove NetworkSettings
- 5. Rename SyncListStruct to SyncListSTRUCT
- 6. Replace NetworkHash128 and NetworkInstanceId
- 8. Replace Components

Others steps are needed to be done by you.

## Installation

Download this repo, and put the inside of Assets\Mirror inside the Mirror folder of your project. 

## Usage

Just go through **Tools\Mirror** on Unity3D with your project open.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)