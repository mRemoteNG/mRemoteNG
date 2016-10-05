---
title: Download v2
permalink: /download2
---

## Beta
{: class='alert alert-warning' }

Former mRemote users, please follow [these instructions]({{ site.github.wiki_url }}/mRemote-Settings-Migration) to migrate your settings from mRemote to mRemoteNG.

{% for release in site.github.releases %}
{% if release.draft == false %}
## {{ release.name }}
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