# mRemoteNG.github.io
mRemoteNG GitHub Hosted Web Pages

# Building
1. Create a [personal access token](https://help.github.com/articles/creating-an-access-token-for-command-line-use/) with `public_repo` permissions
2. Set either `JEKYLL_GITHUB_TOKEN`, or `OCTOKIT_ACCESS_TOKEN` to that token.  You can alternativey use a `~/.netrc` file
3. Run

   ```bash
   bundle install
   bundle exec jekyll serve -w --config _config.yaml,_config-dev.yaml
   ```
4. Open http://localhost:4000/mRemoteNG/
