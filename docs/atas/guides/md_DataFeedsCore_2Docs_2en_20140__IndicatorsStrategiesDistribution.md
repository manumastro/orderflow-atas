# Indicators and strategies distribution

Source: https://docs.atas.net/en/md_DataFeedsCore_2Docs_2en_20140__IndicatorsStrategiesDistribution.html

You can grant access to the developed indicators or strategy(modules) for any ATAS user, set the access expiration date and turn off access at any moment of time.

To do it, it is necessary to:

- [Activate an option of loading modules into the Personal Area](./md_DataFeedsCore_2Docs_2en_20140__IndicatorsStrategiesDistribution.md#title1)

- [Prepare the module](./md_DataFeedsCore_2Docs_2en_20140__IndicatorsStrategiesDistribution.md#title2)

- [Load it into the service](./md_DataFeedsCore_2Docs_2en_20140__IndicatorsStrategiesDistribution.md#title3)

To activate the option of loading module, you need:

- To be registered in the [Personal Area](https://my.atas.net/shop/plans).

- To activate the Activate the Module Section option in the Profile -> For Developers section.

- The My Modules section will appear in the left menu after that. In this section, you can add new module, set users, who have access to these modules, and access expiration.

Preparation of a module Every module, which would be distributed among other users, should have a unique identifier. The [FeatureId](../api/classes/classATAS_1_1Indicators_1_1Attributies_1_1FeatureId.md) attribute is designed for it. This attribute should be assigned to each module, which is included into the distributed package, specifying a unique string identifier.

Example:

[FeatureId("DFD43423-6645-4490-B5F7-45579FF940EE")]

public class MyIndicator : Indicator

{

 //your code

}

One and the same identifier could be assigned to several modules. In such a case, you will be able to control the access of the whole group of modules from the Personal Area.

 After the module is ready, it is necessary to compile it. We also recommend obfuscating the module for its protection.

Loading the module into the service:

- Go to the Modules section

- Press the "Add module" button

- Fill in all the fields in the resulting form:
Module name - name

- Module UID - the identifier, specified in the FeatureId attribute

- Description - arbitrary description

- File - select the compiled dll file

After creating the module, you get the link to the dll file loaded to the service.

 You can send this link to your users for downloading the module.

 Also, for each such module, you can specify users, who have access to it, and the date of access expiration.

 If a user has a downloaded module but has no access or access expired, the user will not see this module in the platform and will not be able to use it.

&zwj;Currently, we do not add any additional copy protection to files provided via the [ATAS](../api/namespaces/namespaceATAS.md) API on our site. This means the developer is currently responsible for protecting the source code. In this context, the obfuscation is indeed the recommended and common approach.When implemented correctly and professionally, it provides sufficient protection against the code being read or redistributed in practice.
