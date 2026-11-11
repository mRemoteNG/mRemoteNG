*****************************************
Protecting the Connections File with DPAPI
*****************************************
.. versionadded:: v1.78.2

Why
---

The connections file (``confCons.xml``) is normally encrypted with a password-derived AES key.
If you never set a master password, mRemoteNG falls back to a **fixed, publicly-known default
password**. Anyone who gets a copy of your ``confCons.xml`` — backed up somewhere, copied,
exfiltrated — can decrypt every stored connection password in it within seconds using publicly
available tools.

Windows DPAPI (`Data Protection API <https://learn.microsoft.com/en-us/windows/win32/seccrypto/data-protection-api>`_)
ties the encryption key to the current Windows user account on the current machine — no master
password to remember or forget to set, and a copied file is cryptographically unreadable
anywhere else (wrong account, wrong machine). No custom cryptography is involved — this is a thin
wrapper around the OS's own ``ProtectedData.Protect``/``Unprotect`` API.

How to turn it on
------------------

Open **Options → Security** and check **"Protect connections file with this Windows user account
(DPAPI) — no master password, not portable to another PC or account"**.

.. figure:: /images/security_options_dpapi_checkbox.png

   The new DPAPI checkbox on the Security options page.

The next time connections are saved, the file is re-written with DPAPI protection and a new
``KeyProtection="Dpapi"`` marker on the root ``<Connections>`` element. Opening it afterwards
needs no master-password dialog at all — mRemoteNG detects the marker and unprotects the file
directly.

What gets encrypted
--------------------

Turning DPAPI on changes **how** encryption happens, not **what** gets encrypted — that is still
controlled by the existing **"Encrypt complete connection file"** checkbox on the same page,
independently:

- **"Encrypt complete connection file" off (default):** DPAPI protects only the password fields
  of each connection (``Password``, ``VNCProxyPassword``, ``RDGatewayPassword``,
  ``RDGatewayAccessToken``). The rest of the XML — hostnames, ports, descriptions — stays plain
  text, same as with the default AES setup.
- **"Encrypt complete connection file" on:** the same password fields are protected as above, and
  the entire XML document content is additionally wrapped in one more layer of DPAPI protection.

What this does NOT do
----------------------

- **No automatic migration.** Turning the checkbox on only affects future saves. Existing
  AES-encrypted connections keep working exactly as before until you explicitly re-save with
  DPAPI turned on. There is no "re-encrypt everything now" button.
- **Not portable.** A DPAPI-protected ``confCons.xml`` can only be opened by the same Windows user
  account on the same machine that saved it. Opening it elsewhere (different account, different
  PC, after a PC reinstall) throws a clear error —
  ``This connections file was protected with Windows DPAPI under a different Windows user account
  or on a different computer, and cannot be decrypted here.`` This is by design, not a bug to work
  around: back up a DPAPI-protected file the same way you'd back up anything else bound to this
  Windows profile, or switch back to a password-based master key before moving to another machine.
- **Doesn't protect against a compromised account.** Anyone logged into the same Windows account
  on the same machine can still open the file — the same limitation as any OS-level "remember me"
  mechanism.

Centrally enforcing DPAPI via Windows Registry
-----------------------------------------------

Like the other Security options on this page, ``UseDpapiProtection`` can be pushed centrally via
the registry (e.g. Group Policy) to enforce it for all users on a machine and disable the
checkbox in the UI. See :doc:`../registry/security_settings` for the registry key and value.
