---
title: Contribute
permalink: /contribute
---
<style>
	#submitBtn {
		transition: opacity 0.35s ease;
	}
</style>
<script>
	$(document).ready(function() {
		var cleave = new Cleave('#amount', {
			numeral: true
		});
		$('#amount').keyup(function(evt) {
			$('#submitBtn').prop('disabled', ($(this).val() ? false : true));
		});
	});
</script>
## Donate
If you find mRemoteNG useful and would like to contribute, it would be greatly appreciated.  When you contribute, you make it possible for the team to cover the costs of producing mRemoteNG.
<div class='card-deck text-xs-center'>
	<div class='card card-block'>
		<h2 class='card-title'>PayPal</h2>
		<form action='https://www.paypal.com/cgi-bin/webscr' method='post'>
			<input type='hidden' name='cmd' value='_donations'>
			<input type='hidden' name='charset' value='utf-8'>
			<input type='hidden' name='business' value='QANUEL2A38KFQ'>
			<input type='hidden' name='return' value='{{ site.url }}{{ site.baseurl }}'>
			<input type='hidden' name='cancel_return' value='{{ site.url }}{{ site.baseurl }}'>
			<input type='hidden' name='currency_code' value='USD'>
			<input type='hidden' name='image_url' value='{{ site.url }}{{ site.baseurl }}/favicon/256.png'>
			<input type='hidden' name='no_shipping' value='1'>
			<div class='form-group'>
				<input type='text' class='form-control' name='item_name' value='mRemoteNG Contribution' readonly>
			</div>
			<div class='form-group'>
				<div class='input-group'>
					<div class='input-group-addon'>$</div>
					<input type='text' class='form-control' id='amount' name='amount' placeholder='Amount' autocomplete='off'>
					<div class='input-group-addon'>USD</div>
				</div>
			</div>
			<button type='submit' class='btn btn-block btn-primary' id='submitBtn' disabled>Donate</button>
		</form>
	</div>
	<div class='card card-block'>
		<h2 class='card-title'>Bitcoin</h2>
		<p class='card-text'>You may also make a contribution by sending <a href='https://www.bitcoin.org/'>Bitcoins</a> to <a href='bitcoin:16fUnHUM3k7W9Fvpc6dug7TAdfeGEcLbSg'><code style='word-break: break-word;'>16fUnHUM3k7W9Fvpc6dug7TAdfeGEcLbSg</code></a>.</p>
		<p class='card-text'><img class='img-responsive' alt='Bitcoin QR Code' src='{{ site.baseurl }}/16fUnHUM3k7W9Fvpc6dug7TAdfeGEcLbSg.png' srcset='{{ site.baseurl }}/16fUnHUM3k7W9Fvpc6dug7TAdfeGEcLbSg.svg' height='100px'>
		</p>
	</div>
</div>

The mRemoteNG Project is **NOT** considered a non-profit organization and contributions are **NOT** tax deductible.

## Submit code
Check out our [source code]({{ site.github.repository_url }}) and submit a pull request or two

## Translate
Check out the [Wiki page]({{ site.github.wiki_url }}/How to Help Translating mRemoteNG) on how to help make mRemoteNG a polygot
