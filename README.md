# dynperf

Linux utility to kill some tasks while some other tasks are running

This application watches when a defined process is running, kills another process and restores it when the defined process is no longer running.
This was created so I can kill compton (window compositor) when playing video games, because it introduces unnecessary input lag and I don't need the fancy visuals while playing.

## Usage

Configuration files are saved in `~/.local/share/dynperf` that includes:

### targets.json

Contains a list of target processes (games, for example) to watch

``Note: This file is watched at runtime, so after updating targets, you do not need to restart dynperf.``

`ProcessName` : Process name for which dynperf will look
`Description` : Optional process description, because sometimes process names may be vague

## Requirements

- This application is targeting `.NET Core 3.1`
