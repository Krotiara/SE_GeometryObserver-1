using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_GeometryObserver.Interfaces
{
    public enum GeomAndCoordsCompareResult
    {
        [Display(Name = "Совпадает")]
        Match,
        //[Display(Name = "Не совпадает")]
        //Mismatch,
        //[Display(Name = "Добавлен")]
        //Added,
        //[Display(Name = "Не найден")]
        //NotFound,
        [Display(Name ="Геометрия совпадает, положение - нет")]
        GeometryEqualButCoordsNot,
        [Display(Name = "Положение совпадает, геометрия - нет")]
        CoordsEqualButGeometryNot,
        [Display(Name ="Не найдена совпадающая геометрия")]
        NotFoundEqual
    }


    public enum GeomCompareResult
    {
        Match,
        Mismatch
    }
}
