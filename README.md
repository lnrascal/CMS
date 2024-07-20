Simplified Version Of A Car Mechanic Simulator

Features:
- Details Should Be Installed To Vehicle In An Order, Which Is Implemented Using A Tree Structure, i.e. Player Can Install A Particular Detail Only If Its Parent Detail Is Installed
- Additionally Different Items Can Be Placed On The Same Node, Which Allows A Player To Customize A Car Using Different Parts
- There Can Be Different Vehicles With Different Structure And Different Items As Well As Share Some Parts With Each Other
- Vehicle Creator Is Implemented, Which Creates A Scriptable Obejct For Each Node, Part, Its Prefab (Also Added To Addressable Group) And Filters Them By Categories
- Vehicle Store/Inventory Allows To Choose Different Category And Car To Filter Catalog
- IInteractable Interface Is Implemented For Interaction With Details And Equipment Such As Car Lift
- OnPickUp Of A Detail Appears A Hint (Through Interaction With Which Detail Can Be Installed To Vehicle) Of That Object On A Vehicle If Details Parent Detail Is Installed
- Detail Components Are Written Using Inheritance, Which Allows To Add Special Detail Components To Make Gameplay More Realistic, ex: Tyres, which should be installed with Rim Right Away (Not Implemented) or Hood Which May Have An Additional Interaction To Open And Close It

