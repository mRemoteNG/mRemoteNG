The aim of this file is to acknowledge the software projects that have been used to create NSubstitute, particularly those distributed as Open Source Software. They have been invaluable in helping us produce this software.

# Software distributed with/compiled into NSubstitute

## Castle.Core
NSubstitute is built on the Castle.Core library, particularly Castle.DynamicProxy which is used for generating proxies for types and intercepting calls made to them so that NSubstitute can record them. 

Castle.Core is maintained by the Castle Project [http://www.castleproject.org/] and is released under the Apache License, Version 2.0 [http://www.apache.org/licenses/LICENSE-2.0.html].

# Software used to help build NSubstitute

## NUnit [http://www.nunit.org/]
NUnit is used for coding and running unit and integration tests for NSubstitute. It is distributed under an open source zlib/libpng based license [http://www.opensource.org/licenses/zlib-license.html].

## Rhino Mocks [http://www.ayende.com/projects/rhino-mocks.aspx]
Used for mocking parts of the NSubstitute mocking framework for testing. It is distributed under the BSD license [http://www.opensource.org/licenses/bsd-license.php].

## Moq [http://moq.me/]
Moq is not used in NSubstitute, but was a great source of inspiration. Moq pioneered Arrange-Act-Assert (AAA) mocking syntax for .NET, as well as removing the distinction between mocks and stubs, both of which have become important parts of NSubstitute. Moq is available under the BSD license [http://www.opensource.org/licenses/bsd-license.php].

## NuPack [http://nupack.codeplex.com/]
Used for packaging NSubstitute for distribution as a nu package. Distributed under the Apache License, Version 2.0 [http://www.apache.org/licenses/LICENSE-2.0.html].

## Jekyll [http://jekyllrb.com/]
Static website generator written in Ruby, used for NSubstitute's website and documentation. Distributed under the MIT license [http://www.opensource.org/licenses/bsd-license.php].

## SyntaxHighlighter [http://alexgorbatchev.com/SyntaxHighlighter/]
Open source, JavaScript, client-side code highlighter used for highlighting code samples on the NSubstitute website. Distributed under the MIT License [http://en.wikipedia.org/wiki/MIT_License] and the GPL [http://www.gnu.org/copyleft/lesser.html].

## FAKE [http://fsharp.github.io/FAKE/]
FAKE (F# Make) is used for NSubstitute's build. It is inspired by `make` and `rake`. FAKE is distributed under a dual Apache 2 / MS-PL license [https://github.com/fsharp/FAKE/blob/master/License.txt].

## Microsoft .NET Framework [http://www.microsoft.com/net/]
NSubstitute is coded in C# and compiled using Microsoft .NET. It can also run and compile under Mono [http://www.mono-project.com], an open source implementation of the open .NET standards for C# and the CLI.

Microsoft's .NET Framework is available under a EULA (and possibly other licenses like MS Reference Source License).
Mono is available under four open source licenses for different parts of the project (including MIT/X11, GPL, MS-Pl). These are described on the project site [http://www.mono-project.com/Licensing].

## Microsoft Ilmerge [http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx]
Used for combining assemblies so NSubstitute can be distributed as a single DLL. Available for use under a EULA as described on the ilmerge site.

## Microsoft Reactive Extensions for .NET (Rx) [http://msdn.microsoft.com/en-us/devlabs/ee794896]
Used to provide .NET 3.5 with some of the neat concurrency helper classes that ship with out of the box with .NET 4.0. Distributed under a EULA [http://msdn.microsoft.com/en-us/devlabs/ff394099].

## 7-Zip [http://www.7-zip.org/]
7-zip is used to ZIP up NSubstitute distributions as part of the automated build process. Distributed under a mixed GNU LGPL / unRAR licence [http://www.7-zip.org/license.txt].

# Other acknowledgements

## Software developers
Yes, you! To everyone who has tried to get better at the craft and science of programming, especially those of you who have talked, tweeted, blogged, screencasted, and/or contributed software or ideas to the community.

No software developers were harmed to any significant extent during the production of NSubstitute, although some had to get by on reduced sleep.

