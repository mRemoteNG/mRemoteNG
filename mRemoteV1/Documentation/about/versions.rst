*****************
Version Numbering
*****************

.. note::

	We are currently rethinking our version numbering scheme and are considering `Semantic Versioning <https://semver.org/>`_
	but will need to discuss it further.

**Our versions follow the format:** ``Major.Minor.Patch.Build``

+-----------+--------------+-----------------------------------------------------------------------+
| Name      | Incremented  | Description                                                           |
+===========+==============+=======================================================================+
| ``Major`` | Manually     | We currently don't increment this number                              |
+-----------+--------------+-----------------------------------------------------------------------+
| ``Minor`` | manually     | | We currently use this number to indicate releases that include new  |
|           |              | | functionality and bug fixes                                         |
+-----------+--------------+-----------------------------------------------------------------------+
| ``Patch`` | Manually     | This number represents the current patch level.                       |
|           |              |                                                                       |
|           |              | Patches are typically released to resolve bugs                        |
+-----------+--------------+-----------------------------------------------------------------------+
| ``Build`` | Manually     | This number represents                                                |
|           |              | ``(number of seconds since midnight)/2``                              |
|           |              |                                                                       |
|           |              | It exists to help prevent version                                     |
|           |              | collisions during development                                         |
+-----------+--------------+-----------------------------------------------------------------------+
