<!---
SPDX-License-Identifier: BSD-3-Clause
SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
SPDX-FileCopyrightText: Z-Wave-Alliance <https://z-wavealliance.org/>
-->
# Z-Wave Tools Core
 This repository contains the essential common tools to build:
 - https://github.com/Z-Wave-Alliance/z-wave-pc-controller
 - https://github.com/Z-Wave-Alliance/z-wave-pc-zniffer
 - https://github.com/Z-Wave-Alliance/z-wave-xml-tools
 - https://github.com/Z-Wave-Alliance/z-wave-automated-test-system
 
 This repository is currently dependent on [z-wave-blobs](https://github.com/Z-Wave-Alliance/z-wave-blobs), which contains a set of pre-compiled files. Going forward, it is intended to evolve Z-Wave Tools Core and the tools related projects [away from Z-Wave Blobs](https://github.com/Z-Wave-Alliance/z-wave-pc-zniffer/issues/6) .
 
OSWG encourages users to avoid duplicating files and introduce a dependency to z-wave-tools-core, and create an issue to [track the propagation of blobs](https://github.com/Z-Wave-Alliance/z-wave-blobs/issues/5).

## Directory Structure
The structure of the tools project is as follows:

```
/root 
    /z-wave-pc-controller
        /z-wave-tools-core
            /z-wave-blobs
        .
        .
    /z-wave-pc-zniffer 
        /z-wave-tools-core
            /z-wave-blobs
        .
        .
    /z-wave-xml-tools
        /z-wave-tools-core
            /z-wave-blobs
        .
        .
```

> NOTE: To pull the submodules, run `git submodule update --init --recursive` from the root directory.

Notice that this structure needs to be kept for the build steps stated below.

## Building 
Z-Wave-tools core can only be built from Windows at the moment. Find below some build steps, assuming that the location is the root directory specified above:
```
del -r */*/bin 
del -r */*/obj
del -r .\Artifacts\

MSBuild.exe z-wave-tools-core\ZWaveDll.sln -restore -verbosity:minimal
MSBuild.exe z-wave-tools-core\ZWaveDll.sln -p:Platform="Any CPU" -p:Configuration=Debug -verbosity:minimal
```
