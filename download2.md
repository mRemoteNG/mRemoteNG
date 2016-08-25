---
title: Download v2
permalink: /download2
---

<div class='alert alert-warning'><h1>Beta</h1></div>

Former mRemote users, please follow [these instructions](https://github.com/mRemoteNG/mRemoteNG/wiki/mRemote-Settings-Migration) to migrate your settings from mRemote to mRemoteNG.

{% for release in site.github.releases %}
{% if release.draft == false %}
# {{ release.name }}
*{{ release.published_at | date: '%c' }}*

```
{{ release.body }}
```
{% for asset in release.assets %}
* [{{ asset.name }}]({{ asset.browser_download_url }}) <span class='badge' title='{{ asset.size }} bytes'>{{ asset.size | divided_by: 1048576.0 | round: 2 }} MiB</span>
{% endfor %}
{% endif %}
{% unless forloop.last %}
---
{% endunless %}
{% endfor %}