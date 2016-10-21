---
title: '[Fireside Chat #2] Onward and Upward'
author: dmsparer
tags: fireside
---

Hey mRemoteNG users!

I'm waaay overdue for a community post, and there's plenty of updates to share.

#### What has happened:
* I'm happy to announce that in early May, Sean Kaim (owner of the mRemote3G fork) joined the development team! Sean has a background in C/Java/C# development. Sean did great work with mRemote3G, and continues to do so with us. Welcome Sean to the team! (Note: The bug fixes that Sean implemented in mRemote3G have been ported over to mRemoteNG. Active work on mRemote3G has been discontinued.)

<!--more-->

* New releases
  * A few weeks ago, we released version 1.74 Release Candidate 2, the first new version of mRemoteNG in over two years. It was a large milestone in reviving the project, and a bit of a scary one at that. Thank you to everyone who has tried the new version and provided feedback to us!
  * We just released 1.74 RC3 today! (I was writing this post as we were prepping the build for release). RC3 fixes a few issues from RC2, such as:
    * Setting LargeAddressAware (this was actually fixed earlier, but for some reason the postbuild action wasn't working correctly)
    * Replace XULrunner with GeckoFX (html renderer for Firefox)
    * Fixed a few resource-not-found issues in the Help window
  * We will give 1.74 RC3 about a week to ensure it is stable. If no major issues are found, we will publish v1.74 as a stable release.
* Here are some of the highlights of v1.74 as a whole:
  * The code base was migrated from VB.Net to C#. This has made the code easier to work with for those of us on the dev team. We are hoping that this also helps us attract more development help.
  * Due to some serious bugs, some custom keyboard hooks that were introduced in v1.73 beta have been removed. This removed the ability to Alt+Tab between connections tabs. We would like to re-add this functionality once we have a stable way to implement it.
  * The installer has been completely redone in WiX (Windows Installer XML). The previous installer was built with NSIS. This gives us a great deal of freedom with the installer. The most prominent change you will notice is that the installer is now a .MSI file, rather than .exe. If there is a desire for it, we can also provide a .exe version of the new installer.
  * The RDP protocol has been updated from RDP 6 to RDP 8. This provided some new features and fixes for RDP connections. With this change, we no longer support installation on Windows XP. Windows 7 is supported with update KB2592687. Windows 8/10 are supported out-of-the-box.
  * XULrunner (the old browser plugin used by mRemoteNG) has been discontinued. Because of this, we have removed support for XULrunner and replaced it with GeckoFX (an implementation of the Firefox redering engine in .Net).
  * We now set the LargeAddressAware flag on the binary. This allows mRemoteNG to use more than 2GB of addressable space. The primary effect of this modification will allow users to open more concurrent connections. This also helps reduce 3334 errors when using RDP in certain situations.
* We now have a code signing certificate! This was not available for the RC2 release, but going forward all exe/dll/msi files that are officially provided by the mRemoteNG dev team will be signed. If you receive an executable file that is not signed, please be very careful and only use the file if you trust its source.
  * Executables provided by the mRemoteNG dev team will be signed by David Sparer, with a root CA of StartCom. We may get a class 3 (organization) certificate at some point in the future, but for now the code is signed by myself directly.
  * Users with SmartScreen enabled may still receive a notification that the MSI installer is unrecognized. We did not pay for the extended validation (EV) certificate so it will take a bit for our reputation to improve with the Microsoft security engine. I regret not getting the EV now - but hey, what are ya gonna do... Eventually we will become a trusted publisher.
  * As always, we will be uploading our official builds to VirusTotal. Feel free to check your own copy before installation/use.

#### What we are planning
* The next update (v1.75) will be focused on security improvements. If you have experience with security code review, we'd love to hear from you! The more eyes we have on the code, the more secure it will be.
  * We want to improve the file encryption used when securing your conConfs.xml file with a password.
  * Just as data and presentation should be separate, so too should secure and insecure data be separate. The best way to handle this is to implement a credential manager and save credentials separately from the connections/host data.
  * Don't reinvent the wheel. We can implement a built-in way to save credentials, but there are numerous great solutions out there for credential management that do it better than we ever could. As long as implementation of the credential manager doesn't run into any serious issues, I'd also like to include support for loading credentials from KeePass. Other credential providers will probably be supported in the future. (And once the code is set up for it, everyone will be free to add support for their favorite credential manager)
  * There are also some very serious security concerns around the integration between mRemoteNG and PuTTY. We want to handle this in 2 ways:
    * Fix the integration if possible
    * Provide a different option for SSH connection management (SSH.Net is our most likely candidate. They just released a beta and hopefully a stable release is coming in the near future.) We would still support PuTTY, but it would not be the default for new installations.
* Other features/improvements will be targeted if time permits.
