---
title: '[Fireside Chat #1] The Story So Far'
author: dmsparer
tags: fireside
---

Hello mRemoteNG users!

I'm a bit overdue for one of these update posts. As mentioned in the last post, I am hoping to provide information to the community about once a month about what we are working on and what we are planning for the future. I'll be calling these the "fireside chats".

<!--more-->

The last update was focused largely on long term features and improvements. However, this is not going to be the main focus for the first few releases. Let me start by going through the events since the last update.

#### What has happened:
* A new developer has joined the team. Please welcome Hayato Iriumi! Hayato has a background in C#/.Net software engineering and release management. He will be working with me to both develop mRemoteNG as well as build an operational platform around the application to support testing and release activities. Hayato will be a great asset to the project and I am glad to have him on the team.
* I have been cleaning up our bug/request tracking system Jira. There is a significant backlog of bug reports and feature/improvement requests that all need to be triaged and validated. We have had to make decisions on what will and will not be worked on for the next release. More on this below.
* I have created a Jenkins instance that will be used in our testing and release management workflows. This will help us to ensure we are releasing quality code while reducing the amount of manual work required to release a working version.
* I am currently creating a virtual testing environment for investigating bug reports and performing smoke testing before a release goes live. Once issue with supporting such a widely used application is that we do not always have the correct environment for replicating bugs seen in very specific scenarios. We are testing out Oracle's Ravello system to virtualize our end user testing environments. That way, we can test issues on Windows 7/8/10 environments without needing to image one of our personal computers. This also allows us to snapshot a particular system configuration in order to test bugs before and after a fix has been implemented.
* I have decided to officially switch the programming language of the project to C#. I know in my last post I mentioned a desire to talk with the community before we made the switch, but this is one item that I think just needs to be done.
* We have been working to stabilize the code after a migration from VB.Net to C#. Specifically, Hayato is currently working on some odd GUI display issues that cropped up as a result of the migration.
* We are working to create unit tests for code as we go through the system. This will help give us the confidence to make some of the larger changes we have planned. Without adequate testing, we cannot always be sure that changes in one portion of the application will not affect others. This is one of the most important things we can do for the long term health of the project - most of our other activities have in some way supported testing and validation.


#### What we are planning:
* I can reduce the plan for the near future into one word: stability. mRemoteNG has not had active development (on this fork) for about 2 years now. Because of this, there is a backlog of bugs from previous releases and issues that have cropped up naturally over time as the protocols and environments we use are upgraded. This means that the next few releases are going to be almost exclusively reserved for bug fixes and core functionality improvements. We cannot allow issues such as application crashes, connection errors, and usability bugs to undermine the main purpose of the application. I don't want any to have to say "I love mRemoteNG, but I can't stand using it anymore".
* I have done a significant amount of cleaning of the Jira boards to support to stability work. All improvements/new features that were not in the current develop branch when I took over have unfortunately been pushed out of this release. I have been trying to triage bugs that affect core functionality. I hope to have a solid list of items to be included in version 1.73 soon, though the list may be fluid for this first release. I don't want to delay the first release too long, so I am trying to keep the number of tickets included to a very low number. Better to do some small releases at first to get the ball rolling.
* Documentation. As we redefine the entire development and release process for the app, we will be documenting everything in a way that will be easy to newcomers. I want to make mRemoteNG a project that is as inclusive as possible. If someone wants to program their own feature request, there should be documentation on how to set up their development environment, coding style expectations, test coverage expectations, how pull requests will work, etc. All the way from "I would like to help" to "congratulations, your changes have been accepted!" This will also include non-development ways to contribute, though for the moment that is less of a concern (not trying to discourage anyone, if you'd like to help let us know!).
* We are still unsure of a release date for 1.73. As we cross off items on our to-do list I'm hoping we will be able to announce something.

That is it for the first(ish) edition of our fireside chats. I'm happy to provide a bit of insight into the development effort and share our thoughts of the near future!
