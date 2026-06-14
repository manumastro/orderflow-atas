# Managing access to custom indicators

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20150__AccessToIndicators.html

Thanks to a specially designed public API, you can control other users' access to your individual custom indicators from any external interface. To do that, you don't have to log into your personal account in [ATAS](https://atas.net/).

Before using, place the dll-file with the developed custom indicator in the APPDATA%\ATAS\Indicators folder. Go to the Knowledge Base to find detailed requirements for developing indicators, information about the capabilities of a specialized API for creating indicators, and rules for connecting custom indicators to the [ATAS](https://atas.net/atas-download/) platform.

To provide other users with access to a new indicator using a specialized API:

- Use the POST /seller/modules/upload method to upload the indicator file in the string($binary) format. The maximum file size is 20 MB. The method will provide uploadFileKey as a response.

- Within 5 minutes after uploading the file with the indicator using the POST /seller/modules method, you must add the description of the uploaded indicator to the "My modules" section. When adding a description, you must specify the name of the indicator, a unique UID, a short description, and the uploadFileKey that you got in the previous step. If the indicator and its description have already been added to the list of your modules, you can skip the first two steps.

- Use the GET /seller/modules method to get a list of all the modules you have published and choose the internal ID of the indicator to which you want to give access to other users.

- Using the POST /seller/modules/{id}/subscriptions method, you can add a user to whom you plan to give access to the indicator. At this step, you need to enter the email of the user to whom you are giving access, the period of access indicating the expiration date, and the ID of the module to which access will be given.

The public API for managing access to custom indicators contains the following collection of methods:

- Get all modules – getting all the indicators you have published.

- Create a module – adding a description of a custom indicator to the "My modules" section.

- Get a module by ID – getting an indicator using its internal ID.

- Update a module – updating custom indicator attributes.

- Delete a module – deleting a custom indicator from the list.

- Check UID for uniqueness – checking indicator UID for uniqueness.

- Upload a module and get "uploadFileKey" for a module creating – uploading a file with a custom indicator.

- Add subscribers to a module - giving a user access to your individual indicator.

- Change the expiration date of module subscriptions – changing the expiration date of the user's access to the indicator.

- Get a list of subscribers for a module – getting a list of users who have access to the indicator.

- Block a subscriber for a module – limiting access to the indicator for a user.

- Get all subscriber's modules – getting a list of indicators and expiration dates for a user.

You can find a detailed description of the methods, examples of their use, and possible errors in the following section ["My modules" - "API documentation"](https://addons.atas.net/modules) in your account on the [ATAS](https://atas.net/) website.
