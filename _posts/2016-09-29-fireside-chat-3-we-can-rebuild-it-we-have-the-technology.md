---
title: '[Fireside Chat #3] We can rebuild it, we have the technology'
author: dmsparer
tags: fireside
---

Hello mRemoteNG users!

Here is another extrememly overdue update from the dev team.

We've been hard at work getting v1.75 ready for alpha/beta testing. I think we are getting close(r). This update was slated to be security-focused with many important improvements. While that's still mostly the case, it has been diluted a bit. When we took a look at the areas of the program that we wanted to change over the next few versions, it became clear that the architecture of the application was going to be a significant burden. Much of the code was tightly woven with how things get displayed and how that display should react when data changes.

<!--more-->

#### What has happened:
* Large re-write of anything that touched the TreeView displayed. If it dared to talk about TreeNodes, it was rewriten. Unfortunately, this took up most of the last month.
  * Everything is now based on our business objects (ConnectionInfo objects and their related decendants). Previously, ConnectionInfo objects were first wrapped in TreeNode objects (which handled display concerns and how the tree was built and ordered).
  * Business logic now deals with the business objects directly. We don't have to worry about how the models are displayed, we can focus on what they should do.
  * The UI code was so entrenched throughout the rest of the program that almost every class was modified in some way. Some had minor changes, some were completely rewritten.
  * As of the time of this writing (and with more work yet to do) here are the stats: 9,215 lines were added and 7,125 lines were deleted across 129 different files across 293 commits.
* On the plus side, the new encryption algorithms (using BouncyCastle) are in place on the develop branch. These are not yet used within the application, though thats the next focus after the re-write mentioned above is finished. There will be a forced upgrade of the encryption used on connections files. If you are encrypting your confCons (you should), there will be a one-time upgrade to use the newer encryption. There will be several options, with AES-GCM being the default. We need folks to check the implementation - if you find any flaws please let us know immediately.
* We've added a bunch of new unit/integration tests. We aren't even close to having the whole project covered, but we've got a start.
* Bennett Blodinger has built out a new website powered by GitHub Pages (thanks Bennett!). In the coming weeks we will re-point the mremoteng.org domain to this new site. You can view the new mock-up here: [https://mremoteng.github.io/mRemoteNG/](https://mremoteng.github.io/mRemoteNG/)
* We are moving all bug/request tracking from Jira to GitHub Issues. Please create new bug/request tickets here: [https://github.com/mRemoteNG/mRemoteNG/issues](https://github.com/mRemoteNG/mRemoteNG/issues). We will still accept Jira tickets for the time being, though the ability to create new tickets will be turned off sometime soon-ish. After that, we will migrate existing tickets from Jira to GitHub. There are a couple of reasons that we decided to make the switch:
  * Jira is one more account/system between you and getting your issue addressed.
  * GitHub Issues will integrate better with our other solutions (chat/Jenkins/website).
  * GitHub Issues will be more visible to the community (because of the previous two reasons).
  * Jira is a bit more powerful/complex than we need.

#### What we are planning:
* Finish the new encryption algorithms. Once this is done, we will do an alpha release for anyone who wants to help bug test. We will run the alpha/beta releases for as long as necessary to ensure no serious bugs escape. With such a significant change to the codebase, I wouldn't be surprised are there are still some odd ones lurking.
* We may sneak a few more bug fixes into v1.75 if time permits. (Some may have been fixed accidentally through the re-write).
* The credential manager functionality has been pushed back to v1.76. This will require some important changes to critical parts of the application. We need to ensure that the application is performing well before tackling something like this. Not to mention that we don't want to delay v1.75 any more than is required.
* The security concerns with the PuTTY integration still need to be addressed. We want to implement a new SSH provider using SSH.Net to be a default replacement for PuTTY. Unsure if this will happen in 1.76 or later. (Note: We will still be supporting the PuTTY integration for the forseeable future.)
