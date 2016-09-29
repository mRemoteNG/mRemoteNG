---
title: Posts
permalink: /posts
---

{% for post in site.posts %}
## [{{ post.title }}]({{ site.baseurl }}{{ post.url }})
**{{ post.author }}** | {{ post.date | date: '%B %-d, %Y'}}
{% unless forloop.last %}
---
{% endunless %}
{% endfor %}
