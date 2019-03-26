.. highlight:: rst

******************
Markup Style Guide
******************

This page covers the conventions for writing and use of the reStructuredText (RST) markup syntax.

Conventions
===========

- Three space indentation.
- Lines should be less than 120 characters long.
- Use italics for button/menu names.

Headings
========

.. code-block:: rst

  #################
    Document Part
  #################

  ****************
  Document Chapter
  ****************

  Document Section
  ================

  Document Subsection
  -------------------

  Document Subsubsection
  ^^^^^^^^^^^^^^^^^^^^^^

  Document Paragraph
  """"""""""""""""""

.. note::

  *Parts* should only be used for contents or index pages.

.. note::

  Each ``.rst`` file should only have one chapter heading (``*``) per file.


Text Styling
============

See the `overview on reStructuredText <http://www.sphinx-doc.org/en/stable/rest.html>`__
for more information on how to style the various elements of the documentation and
how to add lists, tables, pictures and code blocks.
The `Sphinx reference <http://www.sphinx-doc.org/en/master/usage/restructuredtext/index.html>`__ provides more insight additional constructs.

The following are useful markups for text styling::

  *italic*
  **bold**
  ``literal``

Interface Elements
==================

- ``:kbd:`LMB``` -- keyboard and mouse shortcuts.
- ``*Mirror*`` -- interface labels.
- ``:menuselection:`menu item --> submenu item``` -- menus.


Code Samples
============

There is support for syntax highlighting if the programming language is provided,
and line numbers can be optionally shown with the ``:linenos:`` option::

  .. code-block:: powershell
    :linenos:

    write-host "test"
