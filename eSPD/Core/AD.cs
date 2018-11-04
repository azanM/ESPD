using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.DirectoryServices;

namespace eSPD.Core
{
    public class AD
    {

        public ArrayList AttributeValuesMultiString(string attributeName,
        string objectDn, ArrayList valuesCollection, bool recursive)
        {
            DirectoryEntry ent = new DirectoryEntry(objectDn);
            PropertyValueCollection ValueCollection = ent.Properties[attributeName];
            IEnumerator en = ValueCollection.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current != null)
                {
                    if (!valuesCollection.Contains(en.Current.ToString()))
                    {
                        valuesCollection.Add(en.Current.ToString());
                        if (recursive)
                        {
                            AttributeValuesMultiString(attributeName, "LDAP://trac.astra.co.id" +
                            en.Current.ToString(), valuesCollection, true);
                        }
                    }
                }
            }
            ent.Close();
            ent.Dispose();
            return valuesCollection;
        }
        public ArrayList Groups(string userDn, bool recursive)
        {
            ArrayList groupMemberships = new ArrayList();
            return AttributeValuesMultiString("memberOf", userDn,
                groupMemberships, recursive);
        }
        
        public object membership()
        {
            DirectoryEntry localMachine = new DirectoryEntry
      ("WinNT://" + Environment.MachineName + ",Computer");
            DirectoryEntry admGroup = localMachine.Children.Find
                ("Group IT - Admin", "group");
            object members = admGroup.Invoke("members", null);

            foreach (object groupMember in (IEnumerable)members)
            {
                DirectoryEntry member = new DirectoryEntry(groupMember);

            }
            return members;
        }
        public ArrayList EnumerateOU(string OuDn)
        {
            ArrayList alObjects = new ArrayList();
            try
            {
                DirectoryEntry directoryObject = new DirectoryEntry("LDAP://trac.astra.co.id" + OuDn);
                foreach (DirectoryEntry child in directoryObject.Children)
                {
                    string childPath = child.Path.ToString();
                    alObjects.Add(childPath.Remove(0, 7));
                    //remove the LDAP prefix from the path

                    child.Close();
                    child.Dispose();
                }
                directoryObject.Close();
                directoryObject.Dispose();
            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message.ToString());
            }
            return alObjects;
        }
        public ArrayList Groups()
        {
            ArrayList groups = new ArrayList();
            foreach (System.Security.Principal.IdentityReference group in
                System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                groups.Add(group.Translate(typeof
                    (System.Security.Principal.NTAccount)).ToString());
            }
            return groups;
        }
    }
}