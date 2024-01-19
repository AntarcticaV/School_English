using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using School_English.Interface;

namespace School_English.Class
{
    internal class Filter : IFilter
    {
        private static string _searchString;
        private static int _tagGender;
        private static int _tagSort;
        private static IEnumerable<ExtendedClient> _data;

        public Filter(IEnumerable<ExtendedClient> data, string searchString, int tagGender, int tagSort)
        {
            _searchString = searchString;
            _tagGender = tagGender;
            _tagSort = tagSort;
            _data = data;

        }

        public IEnumerable<ExtendedClient> GetFilterAndSort()
        {
            FilterByGenderFemale();
            FilterByGenderMale();
            SortBySecondName();
            SortByID();
            SortByDateOfLastVisit();
            SortByNumberSession();
            SearchUser();
         
            return _data;
        }

        private static void FilterByGenderMale()
        {
            if (_tagGender == 1)
            {
                _data = _data.Where(i => i.GenderCode == "1");
            }
        }

        private static void FilterByGenderFemale()
        {
            if (_tagGender == 2)
            {
                _data = _data.Where(i => i.GenderCode == "2");
            }
        }

        private static void SortBySecondName()
        {
            if (_tagSort == 0)
            {
                _data=_data.OrderBy(i => i.LastName);
            }
        }
        private static void SortByDateOfLastVisit()
        {
            if (_tagSort == 1)
            {
                _data = _data.OrderByDescending(i => i.DateOfLastVisit);
            }
        }

        private static void SortByNumberSession()
        {
            if (_tagSort == 2)
            {
                _data = _data.OrderByDescending(i => i.NumberOfSessions);
            }
        }
        
        private static void SortByID()
        {
            if (_tagSort == 3)
            {
                _data = _data.OrderBy(i => i.ID);
            }
        }

        private static void SearchUser()
        {
            string[] arraySearchString = _searchString.Split(' ');
            
            switch (arraySearchString.Length)
            {
                case 2:
                {
                    _data = _data.Where(i =>
                        (i.FirstName.Contains(arraySearchString[0]) && i.LastName.Contains(arraySearchString[1])) ||
                        (i.LastName.Contains(arraySearchString[0]) && i.FirstName.Contains(arraySearchString[1])));
                    break;
                }
                case 3:
                {
                    _data = _data.Where(i =>
                        i.FirstName.Contains(arraySearchString[0]) && i.LastName.Contains(arraySearchString[1]) && i.Patronymic.Contains(arraySearchString[2]));
                        break;
                }
                default:
                {
                    _data = _data.Where(i => i.Email.Contains(_searchString) ||
                                             i.FirstName.Contains(_searchString) ||
                                             i.Patronymic.Contains(_searchString) ||
                                             i.Phone.Contains(_searchString) ||
                                             i.LastName.Contains(_searchString));
                        break;
                }
            }
        }

        public int Count()
        {
            return _data.Count();
        }
    }
}
