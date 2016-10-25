---
title: Latest update and plans for the future
author: dmsparer
---

Hello all,

I figure it's time I made a post about what has been going on and what I have planned for the future. My intention is to have these sorts of posts somewhat often (once or twice a month at least) to give the community a sense of where the project is and where it is going.

1. I've been working almost daily since announcing my take-over of the project to refactor and decouple the current code base. The current code is so huge and complex that it is suicide to try to get anything done without trying to entangle things. This needs to happen, and for sanity's sake it needs to happen first. The connection/protocol subsystem will be targeted first.

<!--more-->

2. I want to convert the code base to C# and more away from VB.net. Actually, the conversion has already happened on my personal fork of the code. I used this to better learn the current code base and learn VB.net syntax. However, I want to have a dialog with the community before deciding whether to move the official project over to a different language. Disclaimer: the conversion did cause a few bugs, which is to be expected. Most are fixed, though minor GUI bugs still exist.
3. Update the supported versions of RDP, along with other protocols (VNC is largely broken, and SSH needs some love). There have been numerous protocol updates since the last mRemoteNG (mRNG) update. The refactoring work being done in point #1 (above) is in direct support of this goal. I intend to have sufficient abstract to allow users to choose whichever specific version of RDP (or ssh/vcn/etc) they would like to use. This should help folks on newer systems (Win10, Server 2012) take advantage of the most recent protocols, while not giving up support for the poor souls who must still use Server 2003.
4. Move away from PuTTY. In support of updating SSH and related command line protocols, the goal is to move away from PuTTY as the default SSH provider in support of SSH.Net. SSH.Net is a native .Net library for managing remote connections which has also recently found a new owner. It has been ported over to GitHub from CodePlex (GitHub repo here: https://github.com/sshnet/SSH.NET). Please note, this does not mean that PuTTY will no longer be supported as an SSH/telnet/RAW/whatever provider. PuTTY will be, at least for now, still be supported as an option for future mRNG versions. The goal with SSH.Net is to not -require- PuTTY.
5. Credential manager. Yes. If you have to manage more than a single domain of servers, you have likely been frustrated with your username/password management options. The goal is to have a single credential manager within mRNG where username/password pairs are stored. Connections can use a stored credential pair from the manager, or use a manually entered pair (similar to how things work now.) Inheritance will not go away - the credential manager will just be one more way to manage credentials. This feature will provide the groundwork for user-specific credentials when utilizing SQL Server for team-sharing your connections files. This also opens up support for KeePass/PasswordSafe/etc integration. Why save your credentials twice?
6. Work spaces. Similar to Linux/Mac virtual desktops, the idea is there for virtual work spaces. This will be especially useful for those of your who manage several external customer environments. Load separate connections files (or SQL) into separate work spaces with optionally separate credential management.

Now, some of those items are admittedly a long ways off. I don't want to mislead anyone into thinking this is going to happen quickly. However, I'm excited about working on this project and seeing it grow. I intend to communicate that excitement to you over the next few months as the project picks up speed.
