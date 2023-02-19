
using System.Linq;
using AutoMapper;

namespace Tx.Core.Extensions.AutoMapper
{

    public static class AutoMapperEx
    {
        public static TDestination Map<TSource1, TSource2, TDestination>(this IMapper mapper, TSource1 source1, TSource2 source2)
        {
            var destination = mapper.Map<TSource1, TDestination>(source1);
            return mapper.Map(source2, destination);
        }

        public static TDestination Map<TSource1, TSource2, TSource3, TDestination>(this IMapper mapper, TSource1 source1, TSource2 source2, TSource3 source3)
        {
            var destination = mapper.Map<TSource1, TDestination>(source1);
            var destination2 = mapper.Map<TSource2, TDestination>(source2);
            return mapper.Map(source3, destination2);
        }

        public static TResult MergeInto<TResult>(this IMapper mapper, object item1, object item2)
        {
            return mapper.Map(item2, mapper.Map<TResult>(item1));
        }

        public static TResult MergeInto<TResult>(this IMapper mapper, params object[] objects)
        {
            var res = mapper.Map<TResult>(objects.First());
            return objects.Skip(1).Aggregate(res, (r, obj) => mapper.Map(obj, r));
        }
    }
}
