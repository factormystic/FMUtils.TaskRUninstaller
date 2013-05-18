    _________  ____   _ _   _ _     _____         _   ______ _   _       _           _        _ _           
    |  ___|  \/  | | | | | (_) |   |_   _|       | |  | ___ \ | | |     (_)         | |      | | |          
    | |_  | .  . | | | | |_ _| |___  | | __ _ ___| | _| |_/ / | | |_ __  _ _ __  ___| |_ __ _| | | ___ _ __ 
    |  _| | |\/| | | | | __| | / __| | |/ _` / __| |/ /    /| | | | '_ \| | '_ \/ __| __/ _` | | |/ _ \ '__|
    | |   | |  | | |_| | |_| | \__ \_| | (_| \__ \   <| |\ \| |_| | | | | | | | \__ \ || (_| | | |  __/ |   
    \_|   \_|  |_/\___/ \__|_|_|___(_)_/\__,_|___/_|\_\_| \_|\___/|_| |_|_|_| |_|___/\__\__,_|_|_|\___|_|   
                                                                                                        
                                                                                                        

### A self-deleting scheduled task runner/uninstaller ("runinstaller")
I'm using it as a post-uninstall hook for [ProSnap](https://github.com/factormystic/ProSnap#readme), a ClickOnce app (but you can use it for whatever - see _license.txt_)


### Nuget
    Install-Package FMUtils.TaskRUninstaller

### Usage

When you include it in your project, it'll create `FMUtils.TaskRUninstaller.exe` in your output directory. Ship this file and its sole dependency, [David Hall's Task Scheduler Managed Wrapper](https://nuget.org/packages/TaskScheduler) with your ClickOnce package.

On your application's first run, copy these files to a writable location that will survive uninstallation (for example, your vendor directory in `%AppData%`), and run it with the `install` parameter (see below).

It will install a task in the system Task Scheduler with the `runinstall` parameter, that you can use to start your ClickOnce application on user login. If your application path can't be found (eg, because it's been uninstalled), this utility will remove the Scheduled Task and delete itself and it's parent directory (so be careful where you put it).
