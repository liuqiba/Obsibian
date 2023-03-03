## Writing a Script[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=1 "Edit section: Writing a Script")]

You need to have the Memory Viewer window open and go to "Tools->Auto Assemble" or hit CTRL+A to open the Auto assemble window. When you click "Execute" the code is **not** actually executed, but _assembled_ into machine code. The code is actually executed when you overwrite existing game code and the game executes it in the normal course of playing or when you call CREATETHREAD.

Writing an address or label followed by a colon will do one of two opposite things. If the label is known, i.e. it is an address or if there is a defined symbol or memory has been allocated with that name, the assembler will move to that address for assembling the following code. If the label is unknown, it must have been passed to LABEL(name) (or you will get an error) and the value of that label will be set to the current position where code is set to be assembled.

[Simple Example](https://wiki.cheatengine.org/index.php?title=Auto_Assembler_Example_1 "Auto Assembler Example 1") - Example showing ALLOC, LABEL, REGISTERSYMBOL and CREATETHREAD.

## Assigning a Script to a CheatTable[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=2 "Edit section: Assigning a Script to a CheatTable")]

Scripts assigned to cheat tables usually have two sections, "[ENABLE]" and "[DISABLE]". Code before "[ENABLE]" will be _**assembled**_ every time the script is enabled OR disabled. The code in the "[ENABLE]" section will be _**assembled**_ (not executed) when the entry is checked and the code in the "[DISABLE]" section will be _**assembled**_ when the entry is unchecked.

You will generally alloc memory in [ENABLE] and overwrite existing instructions inside the process you have opened to jump to your code where you can modify values and jump back. You will then dealloc the memory and put the original instructions back when disabling.

To assign it to your cheat table, click on "File->Assign to current cheat table" and close the window because to edit the table script you have to double-click on the "<script>" value in your table.

[Serious Sam 3 BFE Example](https://wiki.cheatengine.org/index.php?title=Auto_Assembler_Example_2 "Auto Assembler Example 2") - Example showing ENABLE and DISABLE

## Injecting a DLL[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=3 "Edit section: Injecting a DLL")]

loadlibrary(name) can be used to load a dll and register it's symbols for use by your assembly code. Note that you should not put quotes around the DLL name. Here's an example:

[LoadLibrary Example](https://wiki.cheatengine.org/index.php?title=Auto_Assembler_Example_3 "Auto Assembler Example 3")

## General Information[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=4 "Edit section: General Information")]

Auto assemble allows you to write assembler code at different locations using a script. It can be found in the memory view part of cheat engine under extra.

See [Auto Assembler Commands](https://wiki.cheatengine.org/index.php?title=Auto_Assembler:Commands "Auto Assembler:Commands") for a full list of all Auto Assembler commands.

<table><caption>Auto Assembler Commands</caption><tbody><tr><th>Command</th><th>Description</th></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:aobScan" title="Auto Assembler:aobScan">AOBSCAN</a>(name, xx xx xx xx xx)</td><td>Scans the memory for the given array of byte and sets the result to the symbol named "name"</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:aobScanModule" title="Auto Assembler:aobScanModule">AOBSCANMODULE</a>(name, moduleName, xx xx xx xx xx)</td><td>Scans the memory of a specific module for the given array of byte and sets the result to the symbol names "name"</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:aobScanRegion" title="Auto Assembler:aobScanRegion">AOBSCANREGION</a>(name, Sadd$, Fadd$, xx xx xx)</td><td>Will scan the specific range from start address to finish addressfor the given AOB and labels it with the given name</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:alloc" title="Auto Assembler:alloc">ALLOC</a>(allocName, sizeInBytes, Optional: AllocateNearThisAddress)</td><td>Allocates a certain amount of memory and defines the specified name in the script. If AllocateNearThisAddress is specified CE will try to allocate the memory near that address. This is useful for 64-bit targets where the jump distance could be bigger than 2GB otherwise</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:dealloc" title="Auto Assembler:dealloc">DEALLOC</a>(allocName)</td><td>Deallocates a block of memory allocated with Alloc. It always gets executed last, no matter where it is positioned in the code, and only actually frees the memory when all allocations have been freed. Only usable in a script designed as a cheat table. (e.g used for the disable cheat)</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:createThread" title="Auto Assembler:createThread">CREATETHREAD</a>(address)</td><td>Will spawn a thread in the process at the specified address</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:define" title="Auto Assembler:define">DEFINE</a>(name,whatever)</td><td>Creates a token with the specified name that will be replaced with the text of whatever</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:fullAccess" title="Auto Assembler:fullAccess">FULLACCESS</a>(address,size,preferedsize)</td><td>Makes a memory region at the specified address and at least "size" bytes readable, writable and executable</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:globalAlloc" title="Auto Assembler:globalAlloc">GLOBALALLOC</a>(name,size)</td><td>Allocates a certain amount of memory and registers the specified name. Using GlobalAlloc in other scripts will then not allocate the memory again, but reuse the already existing memory. (Or allocate it anyhow if found it wasn't allocated yet)</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:include" title="Auto Assembler:include">INCLUDE</a>(filename)</td><td>Includes another auto assembler file at that spot</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:label" title="Auto Assembler:label">LABEL</a>(labelName)</td><td>Enables the word labelName to be used as an address</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:loadBinary" title="Auto Assembler:loadBinary">LOADBINARY</a>(address,filename)</td><td>Loads a binary file at the specified address</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:loadLibrary" title="Auto Assembler:loadLibrary">LOADLIBRARY</a>(filename)</td><td>Injects the specified DLL into the target process</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:readMem" title="Auto Assembler:readMem">READMEM</a>(address,size)</td><td>Writes the memory at the specified address with the specified size to the current location</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:registerSymbol" title="Auto Assembler:registerSymbol">REGISTERSYMBOL</a>(symbolName)</td><td>Adds a symbol to the user-defined symbol list so cheat tables and the memory browser can use that name instead of a address (The symbol has to be declared in the script when using it)</td></tr><tr><td><a href="https://wiki.cheatengine.org/index.php?title=Auto_Assembler:unregisterSymbol" title="Auto Assembler:unregisterSymbol">UNREGISTERSYMBOL</a>(symbolName)</td><td>Removes a symbol from the user-defined symbol list. No error will occur if the symbol doesn't exist.</td></tr></tbody></table>

## Value Notation[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=5 "Edit section: Value Notation")]

Normally everything is written as hexadecimal in auto assembler, but there are ways to override this so you can input decimal values, and even floating point values. for example, a integer value of 100 can be written in hex as 64, but you can also write it as #100, or as (int)100 for floating point value like 100.1 you can use (float)100.1 and for a double, you could use (double)100.1 NOTE:Use 'dq (double)100.1' instead of 'dd'!

### {$lua}[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=6 "Edit section: {$lua}")]

Auto assembler scripts support section written in Lua.You can start such a section using the [{$lua}](https://wiki.cheatengine.org/index.php?title=Auto_Assembler:LUA_ASM "Auto Assembler:LUA ASM") keyword, and end it with [{$asm}](https://wiki.cheatengine.org/index.php?title=Auto_Assembler:LUA_ASM "Auto Assembler:LUA ASM").

The return value of such a function (if it returns a value at all) will be interpreted as normal auto assembler commands.

When syntax checking, the lua sections get executed. To make sure your lua script behaves properly in those situations, check the "syntaxcheck" boolean. If it's true, then do not make permanent changes. e.g:

```
if syntaxcheck then return end


```

Of course, if your script is meant to generate code, do make it return code so that it passes the initial syntax check. (e.g label definitions etc...)

## Examples[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=7 "Edit section: Examples")]

### Basic Example[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=8 "Edit section: Basic Example")]

```
00451029:
jmp 00410000
nop
nop
nop

00410000:
mov [00580120],esi
mov [esi+80],ebx
xor eax,eax
jmp 00451031


```

### Example using LABEL[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=9 "Edit section: Example using LABEL")]

```
label(mylabel)

00451029:
jmp 00410000
nop
nop
nop
mylabel:

00410000:
mov [00580120],esi
mov [esi+80],ebx
xor eax,eax
jmp mylabel


```

### Example using ALLOC[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=10 "Edit section: Example using ALLOC")]

```
alloc(alloc1,4)

00451029:
jmp 00410000
nop
nop
nop

00410000:
mov [alloc1],esi
mov [esi+80],ebx
xor eax,eax
jmp 00451031


```

### Example using ALLOC and LABEL[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=11 "Edit section: Example using ALLOC and LABEL")]

```
alloc(alloc1,4)
label(mylabel)

00451029:
jmp 00410000
nop
nop
nop
mylabel:

00410000:
mov [alloc1],esi
mov [esi+80],ebx
xor eax,eax
jmp mylabel


```

### Example using FULLACCESS[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=12 "Edit section: Example using FULLACCESS")]

```
FULLACCESS(00400800,4) //00400800 is usually read only non executable data, this makes it writeable and executable
00451029:
jmp 00410000
nop
nop
nop

00410000:
mov [00400800],esi
mov [esi+80],ebx
xor eax,eax
jmp 00451031


```

### Example using DEFINE[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=13 "Edit section: Example using DEFINE")]

```
DEFINE(clear_eax,xor eax,eax)
00400500:
clear_eax


```

### Example using READMEM[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=14 "Edit section: Example using READMEM")]

```
alloc(x,16)
alloc(script,2048)

script:
mov eax,[x]
mov edx,[x+c]
ret

x:
readmem(00410000,16) //place the contents of address 00410000 at the address of X


```

## See also[[edit](https://wiki.cheatengine.org/index.php?title=Cheat_Engine:Auto_Assembler&action=edit&section=15 "Edit section: See also")]

*   [Assembler](https://wiki.cheatengine.org/index.php?title=Assembler "Assembler")
*   [Tutorials](https://wiki.cheatengine.org/index.php?title=Tutorials "Tutorials")

NewPP limit report Cached time: 20230207210506 Cache expiry: 86400 Dynamic content: false CPU time usage: 0.044 seconds Real time usage: 0.046 seconds Preprocessor visited node count: 131/1000000 Preprocessor generated node count: 224/1000000 Post‐expand include size: 0/2097152 bytes Template argument size: 0/2097152 bytes Highest expansion depth: 2/40 Expensive parser function count: 0/100 Unstrip recursion depth: 0/20 Unstrip post‐expand size: 1060/5000000 bytes

Transclusion expansion time report (%,ms,calls,template) 100.00% 0.000 1 -total

Saved in parser cache with key wikidb-cew_:pcache:idhash:47-0!canonical and timestamp 20230207210506 and revision id 6829