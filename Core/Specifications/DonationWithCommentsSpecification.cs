using Core.Entities;

namespace Core.Specifications
{

    // public class DonationWithCommentsSpecification : BaseSpecification<Donation>
    //{
    //    public DnationWithCommentsSpecification(DonationSpecParams donationParrams) 
    //        : base(x => 
    //            (string.IsNullOrEmpty(donationParrams.Search) || x.DonationTypeName.ToLower().Contains(donationParrams.Search))
    //        )
    //    {
    //        AddInclude(x => x.DonationTypeName);
    //        AddOrderBy(x => x.SetOn);
    //        ApplyPaging(donationParrams.PageSize * (donationParrams.PageIndex - 1), donationParrams.PageSize);

    //        // if (!string.IsNullOrEmpty(donationParrams.Sort))
    //        // {
    //        //     switch (donationParrams.Sort)
    //        //     {
    //        //         case "priceAsc":
    //        //             AddOrderBy(p => p.Price);
    //        //             break;
    //        //         case "priceDesc":
    //        //             AddOrderByDescending(p => p.Price);
    //        //             break;
    //        //         default:
    //        //             AddOrderBy(n => n.Name);
    //        //             break;
    //        //     }
    //        // }
    //    }

    //    public DonationWithCommentsSpecification(int id) 
    //        : base(x => x.Id == id)
    //    {
    //        AddInclude(x => x.DonationTypeName);
    //    }
    //}
    public class DonationWithCommentsSpecification : BaseSpecification<Donation>
    {
        public DonationWithCommentsSpecification(DonationSpecParams donationParrams)
            : base(x =>
                (string.IsNullOrEmpty(donationParrams.Search) || x.DonationTypeName.ToLower().Contains(donationParrams.Search))
            )
        {
            //AddInclude(x => x.DonationTypeName);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(donationParrams.PageSize * (donationParrams.PageIndex - 1), donationParrams.PageSize);

            // if (!string.IsNullOrEmpty(postParrams.Sort))
            // {
            //     switch (postParrams.Sort)
            //     {
            //         case "priceAsc":
            //             AddOrderBy(p => p.Price);
            //             break;
            //         case "priceDesc":
            //             AddOrderByDescending(p => p.Price);
            //             break;
            //         default:
            //             AddOrderBy(n => n.Name);
            //             break;
            //     }
            // }
        }

        public DonationWithCommentsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.DonationTypeName);
        }
    }
}
