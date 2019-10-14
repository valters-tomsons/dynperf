# dynperf
Linux utility to kill some tasks while some other tasks are running

This application watches when a defined process is running, kills another process and restores it when the defined process is no longer running.
This was created so I can kill compton (window compositor) when playing video games, because it introduces unnecessary input lag and I don't need the fancy visuals while playing.

# Usage
Configuration files are saved in `~/.local/share/dynperf` that includes:
### config.json
Contains general settings, like restore script (executes a bash command) and process scan interval etc.
    
### targets.json
Contains a list of target processes (games, for example) to watch.    

``Note: This file is watched at runtime, so after updating targets, you do not need to restart dynperf.``

`ProcessName` : Process name for which dynperf will look    
`Description` : Optional process description, because sometimes process names may be vague

# Configuration
Launch `dynperf-server` once, so it can generate the configuration file and default target definition list.

`KillProcess` - Process name to kill when a target process is found -- e.g. `compton`

`RestoreCommand` - Bash command that will be launched when there are none target processes running -- e.g. `compton --config ~/.compton.conf`

# Requirements
- This application is targeting `.NET Core 2.1`
- Bash at `/bin/bash`, this could be changed pretty easily tho
