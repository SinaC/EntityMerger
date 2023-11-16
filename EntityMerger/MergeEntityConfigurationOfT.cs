﻿using System.Linq.Expressions;

namespace EntityMerger.EntityMerger;

internal class MergeEntityConfiguration<TEntity> : IMergeEntityConfiguration<TEntity>
    where TEntity : class
{
    public MergeEntityConfiguration Configuration { get; private set; }

    public MergeEntityConfiguration()
    {
        Configuration = new MergeEntityConfiguration(typeof(TEntity));
    }

    internal MergeEntityConfiguration(MergeEntityConfiguration mergeEntityConfiguration)
    {
        Configuration = mergeEntityConfiguration;
    }

    public IMergeEntityConfiguration<TEntity> HasKey<TKey>(Expression<Func<TEntity, TKey>> keyExpression)
    {
        // TODO: can only be set once
        var keyProperties = keyExpression.GetSimplePropertyAccessList().Select(p => p.Single());
        var equalityComparerByProperties = new EqualityComparerByProperties<TEntity>(keyProperties);

        var config = Configuration.SetKey(keyProperties, equalityComparerByProperties);
        return this;
    }

    public IMergeEntityConfiguration<TEntity> HasKey<TKey>(Expression<Func<TEntity, TKey>> keyExpression, Action<IKeyConfiguration> keyConfigurationAction)
    {
        // TODO: can only be set once
        var keyProperties = keyExpression.GetSimplePropertyAccessList().Select(p => p.Single());
        var equalityComparerByProperties = new EqualityComparerByProperties<TEntity>(keyProperties);

        var config = Configuration.SetKey(keyProperties, equalityComparerByProperties);
        keyConfigurationAction?.Invoke(config);
        return this;
    }

    public IMergeEntityConfiguration<TEntity> HasCalculatedValue<TValue>(Expression<Func<TEntity, TValue>> calculatedValueExpression)
    {
        // TODO: check if value property has not been already registered
        var calculatedValueProperties = calculatedValueExpression.GetSimplePropertyAccessList().Select(p => p.Single());
        var equalityComparerByProperties = new EqualityComparerByProperties<TEntity>(calculatedValueProperties);
        var config = Configuration.SetCalculatedValue(calculatedValueProperties, equalityComparerByProperties);
        return this;
    }

    public IMergeEntityConfiguration<TEntity> HasCalculatedValue<TValue>(Expression<Func<TEntity, TValue>> calculatedValueExpression, Action<ICalculatedValueConfiguration> calculatedValueConfigurationAction)
    {
        // TODO: check if value property has not been already registered
        var calculatedValueProperties = calculatedValueExpression.GetSimplePropertyAccessList().Select(p => p.Single());
        var equalityComparerByProperties = new EqualityComparerByProperties<TEntity>(calculatedValueProperties);
        var config = Configuration.SetCalculatedValue(calculatedValueProperties, equalityComparerByProperties);
        calculatedValueConfigurationAction?.Invoke(config);
        return this;
    }

    public IMergeEntityConfiguration<TEntity> HasMany<TTargetEntity>(Expression<Func<TEntity, ICollection<TTargetEntity>>> navigationPropertyExpression)
        where TTargetEntity : class
    {
        // TODO: must be a of type List<T>
        // TODO: check if navigation property has not been already registered
        var config = Configuration.AddNavigationMany(navigationPropertyExpression.GetSimplePropertyAccess().Single());
        return this;
    }

    public IMergeEntityConfiguration<TEntity> HasOne<TTargetEntity>(Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression)
        where TTargetEntity : class
    {
        // TODO: check if navigation property has not been already registered
        var config = Configuration.AddNavigationOne(navigationPropertyExpression.GetSimplePropertyAccess().Single());
        return this;
    }

    public IMergeEntityConfiguration<TEntity> MarkAsInserted<TMember>(Expression<Func<TEntity, TMember>> destinationMember,
        TMember value)
    {
        var config = Configuration.SetMarkAsInserted(destinationMember.GetSimplePropertyAccess().Single(), value!);
        return this;
    }

    public IMergeEntityConfiguration<TEntity> MarkAsUpdated<TMember>(Expression<Func<TEntity, TMember>> destinationMember, TMember value)
    {
        var config = Configuration.SetMarkAsUpdated(destinationMember.GetSimplePropertyAccess().Single(), value!);
        return this;
    }

    public IMergeEntityConfiguration<TEntity> MarkAsDeleted<TMember>(Expression<Func<TEntity, TMember>> destinationMember, TMember value)
    {
        var config = Configuration.SetMarkAsDeleted(destinationMember.GetSimplePropertyAccess().Single(), value!);
        return this;
    }
}