# [mRemoteNG.github.io/mRemoteNG](https://mRemoteNG.github.io/mRemoteNG/)
mRemoteNG Website

# Building
1. Create a [personal access token](https://help.github.com/articles/creating-an-access-token-for-command-line-use/) with `public_repo` permissions
2. Set either `JEKYLL_GITHUB_TOKEN`, or `OCTOKIT_ACCESS_TOKEN` to that token.  You can alternativey use a `~/.netrc` file
3. Run

   ```bash
   bundle install
   bundle exec jekyll serve --drafts --config _config.yaml,_config-dev.yaml
   ```
4. Open http://localhost:4000/mRemoteNG/
